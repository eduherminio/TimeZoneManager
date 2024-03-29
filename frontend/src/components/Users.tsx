import * as React from 'react';
import { connect } from 'react-redux';
import { RouteComponentProps } from 'react-router';
import { ApplicationState } from '../store';
import * as UsersStore from '../store/Users';
import { UserDto, FullUserDto } from '../server/UserClient';
import { RoleClient, RoleDto } from '../server/RoleClient';
import { Button } from '../formcontrols/Button/Button';
import tableIcons from '../formcontrols/tableIcons';
import MaterialTable from 'material-table';
import { RoleName } from '../server/autogeneratedclients/TimeZoneManagerClient';
import { sha512 } from 'js-sha512';
import { CircularProgress } from '@material-ui/core';

type rowType = { key: string, username: string; roleDescription: string; roleName: string; password: string }

// At runtime, Redux will merge together...
type UsersProps =
  UsersStore.UsersState // ... state we've requested from the Redux store
  & typeof UsersStore.actionCreators // ... plus action creators we've requested
  & RouteComponentProps<{}>; // ... plus incoming routing parameters

interface UserState {
  roles: Array<RoleDto>,
  errors: {
    username: string,
    password: string,
  }
}

class Users extends React.PureComponent<UsersProps, UserState> {
  constructor(props: UsersProps) {
    super(props);

    this.state = {
      roles: [],
      errors: {
        username: '',
        password: ''
      }
    }

    this.ensureDataFetched = this.ensureDataFetched.bind(this);
    this.handleChange = this.handleChange.bind(this);
    this.renderUsersTable = this.renderUsersTable.bind(this);
    this.validateUserData = this.validateUserData.bind(this);
  }

  private roleDictionary: { [id: string]: RoleName } = {
    '0': RoleName.Unknown,
    '1': RoleName.User,
    '2': RoleName.UserManager,
    '3': RoleName.Admin,
  }

  private readonly columns = [
    { title: 'Key', field: 'key', editable: 'never' as const, hidden: true },
    { title: 'Username', field: 'username', editable: 'always' as const },
    {
      title: 'Role name', field: 'roleName', editable: 'always' as const,
      lookup: {
        0: 'None',
        1: RoleName.User.toString(),
        2: RoleName.UserManager.toString(),
        3: RoleName.Admin.toString(),
      },
      initialEditValue: 'none'
    },
    { title: 'Role', field: 'roleDescription', editable: 'never' as const },
    { title: 'Password', field: 'password', editable: 'onAdd' as const },
  ]

  private GenerateDataRow(user: UserDto): rowType {
    return {
      key: user.key ? user.key : '',
      username: user.username,
      roleDescription: user.roles ? user.roles.map(r => `${r.description}`).join(', ') : '',
      roleName: user.roles && user.roles.length > 0 ? Object.values(this.roleDictionary).indexOf(user.roles[0].name as RoleName).toString() : RoleName.Unknown.toString(),
      password: '*',
    };
  }

  // This method is called when the component is first added to the document
  public componentDidMount() {
    let client: RoleClient = new RoleClient();
    client.loadAll()
      .then(roles => this.setState({ roles: roles }))
      .catch(error => this.setState({ errors: { username: error, password: '' } }));
    this.ensureDataFetched();
  }

  public handleChange(e: any): void {
    this.ensureDataFetched();
  }

  private ensureDataFetched() {
    this.props.requestUsers();
  }

  public render() {
    return (
      <React.Fragment>
        <h1 id="tabelLabel">Users</h1>
        <div onClick={this.handleChange}>
          <Button
            value={'Refresh'}
            type={'submit'} />
        </div>
        <br />
        {this.renderUsersTable()}
        {this.renderError()}
      </React.Fragment>
    );
  }

  private renderError() {
    return (
      <div>
        <div className={"panelRowError"}>
          <ul className="ul">
            <li>{this.state.errors.username}</li>
            <li>{this.state.errors.password}</li>
          </ul>
        </div>

        <div className={"panelRowError"}>
          <span>{this.props.errorMessage}</span>
        </div>
      </div>)
  }

  private renderUsersTable() {
    if (this.props.isLoading) {
      return <CircularProgress />;
    }

    return (
      <MaterialTable
        title={'Existing users'}
        icons={tableIcons}
        columns={this.columns}
        data={this.props.users.map((user: UserDto) => {
          return this.GenerateDataRow(user)
        })}
        editable={{
          onRowAdd: newData =>
            this.onRowAdd(newData),
          onRowUpdate: (newData, oldData) =>
            this.onRowUpdate(newData, oldData),
          onRowDelete: oldData =>
            this.onRowDelete(oldData)
        }}
      />);
  }


  private validateUserData(data: rowType) {
    let errors = {
      username: '',
      password: ''
    }

    errors.username = !data.username || data.username.replace(' ', '') !== data.username
      ? 'Please select a username (no spaces allowed)'
      : '';

    errors.password = !data.password
      ? 'Please select a password'
      : '';

    this.setState({ errors: errors });

    return errors;
  }

  private onRowAdd(newData: rowType): Promise<void> {
    let errors = this.validateUserData(newData);

    if (errors.username || errors.password) {
      return new Promise(resolve => resolve());
    }

    return new Promise(resolve => {
      setTimeout(() => {
        let timeZone: FullUserDto = this.generateFullUserDto(newData);
        this.props.createUser(timeZone);
        resolve();
      }, 1000);
    });
  }

  private onRowUpdate(newData: rowType, oldData: rowType | undefined): Promise<void> {
    let errors = this.validateUserData(newData);

    if (errors.username || errors.password) {
      return new Promise(resolve => resolve());
    }

    return new Promise(resolve => {
      setTimeout(() => {
        if (oldData) {
          let userKey: string = this.extractSelectedUserKey(oldData);
          if (userKey) {
            let user: UserDto = this.generateUserDto(newData);
            user.key = userKey;
            this.props.updateUser(user);
          }
        }
        resolve();
      }, 1000);
    });
  }

  private onRowDelete(oldData: rowType): Promise<void> {
    return new Promise(resolve => {
      setTimeout(() => {

        let userKey: string = this.extractSelectedUserKey(oldData);

        this.props.deleteUser(userKey);
        resolve();
      }, 1000);
    });
  }

  private generateUserDto(newData: rowType): UserDto {
    return {
      username: newData.username,
      roles: this.extractSelectedRole(newData)
    };
  }

  private generateFullUserDto(newData: rowType): FullUserDto {
    let password: string = newData.password
      ? sha512(newData.password)
      : '';

    return {
      username: newData.username,
      roles: this.extractSelectedRole(newData),
      password: password
    };
  }

  private extractSelectedUserKey(oldData: rowType) {
    let userKey: string = '';
    if (oldData.key) {
      userKey = oldData.key;
    }
    else {
      let filteredUser = this.props.users.filter(u => u.username === oldData.username);
      if (filteredUser.length > 0 && filteredUser[0].key) {
        userKey = filteredUser[0].key;
      }
    }
    return userKey;
  }

  private extractSelectedRole(newData: rowType): RoleDto[] | undefined {
    let roleName: string = this.roleDictionary[newData.roleName] ? this.roleDictionary[newData.roleName].toString() : '';
    return this.state.roles.filter(r => r.name === roleName);
  }
}

export default connect(
  (state: ApplicationState) => state.users, // Selects which state properties are merged into the component's props
  UsersStore.actionCreators // Selects which action creators are merged into the component's props
)(Users as any);
