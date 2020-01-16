import * as React from 'react';
import { connect } from 'react-redux';
import { RouteComponentProps, Redirect } from 'react-router';
import { JwtManager } from '../server/JwtManager/JwtManager';
import * as SessionStore from '../store/Session';
import { ApplicationState } from '../store';
import { ExistingPages } from '../ExistingPages';

type LogoutProps =
  SessionStore.SessionState // ... state we've requested from the Redux store
  & typeof SessionStore.actionCreators // ... plus action creators we've requested
  & RouteComponentProps<{}>; // ... plus incoming routing parameters

interface LogoutState {
  disconnect: boolean
}

class Logout extends React.Component<LogoutProps, LogoutState> {

  private readonly jwtManager: JwtManager = new JwtManager(null);

  constructor(props: LogoutProps) {
    super(props);

    this.state = { disconnect: false }

    this.updateState = this.updateState.bind(this);
  }

  private updateState() {
    this.jwtManager.forget();
    this.props.clearSession();

    this.setState({ disconnect: true });
  }

  render(): JSX.Element {
    return this.state.disconnect
      ? this.renderLoginRedirect()
      : this.renderLogoutMessage();
  }

  private renderLogoutMessage(): JSX.Element {
    this.updateState();
    return (<div>
      <h4>Logging you out...</h4>
    </div>);
  }

  private renderLoginRedirect(): JSX.Element {
    return <Redirect to={ExistingPages.login}></Redirect>;
  }
}

export default connect(
  (state: ApplicationState) => state.session, // Selects which state properties are merged into the component's props
  SessionStore.actionCreators // Selects which action creators are merged into the component's props
)(Logout as any);