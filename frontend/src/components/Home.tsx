import * as React from 'react';
import { connect } from 'react-redux';
import { Redirect } from 'react-router';
import { ApplicationState } from '../store';
import { ExistingPages } from '../ExistingPages';
import { JwtManager } from '../server/JwtManager';
import * as SessionStore from '../store/Session';

interface HomeStoreProps {
    session: SessionStore.SessionState | undefined;
}

interface HomeState{
    loggedIn: boolean;
}

class Home extends React.Component<HomeStoreProps, HomeState> {
    private readonly jwtManager = new JwtManager(null);

    constructor(props: HomeStoreProps) {
      super(props);

      this.state = {loggedIn: true};
    }
    public componentDidMount() {
      this.ensureExistingSession();
    }

    public componentDidUpdate() {
      this.ensureExistingSession();
    }
  
    private ensureExistingSession() {
      if(!this.props.session || ! (this.props.session.permissions.length > 0))
      {
        this.jwtManager.forget();
        this.setState({loggedIn: false})
      }
    }

    public render(): JSX.Element {
      return this.state.loggedIn
      ? (
      <div>
        <h1>TimeZone Manager</h1>
        <img src={require('../resources/TimeZonePicture.png')} alt="Time zones"/>
      </div>)
      : <Redirect to={ExistingPages.login} />;
    }
}

function mapStateToProps(state: ApplicationState): HomeStoreProps {
    return {
        session: state.session
    };
}

export default connect(mapStateToProps)(Home as any);