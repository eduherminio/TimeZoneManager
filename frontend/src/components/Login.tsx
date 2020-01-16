import * as React from 'react';
import { connect } from 'react-redux';
import { Redirect, RouteComponentProps } from 'react-router';
import { sha512 } from 'js-sha512';
import { JwtManager, JwtTokenPayload } from '../server/JwtManager/JwtManager';
import { LoginClient } from '../server/LoginClient'
import * as SessionStore from '../store/Session';
import { ApplicationState } from '../store';
import { TextField, Typography } from '@material-ui/core';
import { Button } from '../formcontrols/Button/Button';
import { ExistingPages } from '../ExistingPages';
import './FormStyles.css';

type LoginProps =
    SessionStore.SessionState // ... state we've requested from the Redux store
    & typeof SessionStore.actionCreators // ... plus action creators we've requested
    & RouteComponentProps<{}>; // ... plus incoming routing parameters

interface LoginState {
    username?: string;
    password?: string;
    errorText?: string;
    loading?: boolean;
    redirectToRegister?: boolean;
    errors: {
        username: string,
        password: string
    }
}

class Login extends React.Component<LoginProps, LoginState> {

    private readonly jwtManager: JwtManager = new JwtManager(null);

    constructor(props: LoginProps) {
        super(props);

        this.state = { username: '', password: '', errorText: '', loading: false, redirectToRegister: false, errors: { username: '', password: '' } };

        this.handleChange = this.handleChange.bind(this);
        this.submitLogin = this.submitLogin.bind(this);
        this.register = this.register.bind(this);
        this.clearError = this.clearError.bind(this);
        this.isNotReadyToLogin = this.isNotReadyToLogin.bind(this);
    }

    handleChange(e: any): void {
        const { name, value } = e.target;
        this.setState({ ...this.state, [name]: value });

        let errors = this.state.errors;
        switch (name) {
            case 'username': {
                errors.username = value
                    ? ''
                    : 'Please enter your username'
                break;
            }
            case 'password': {
                errors.password = value ? '' : 'Please enter your password'
                break;
            }
        }

        this.setState({ errors: errors });
    }

    private isNotReadyToLogin(): boolean {
        return (this.state.loading ? this.state.loading : false)
            || !this.state.username
            || !this.state.password
            || this.state.errors.username !== ''
            || this.state.errors.password !== '';
    }

    submitLogin(e: any): void {
        if (this.isNotReadyToLogin()) {
            return;
        }

        e.preventDefault();
        this.setState({ loading: true });

        let password: string = this.state.password
            ? sha512(this.state.password)
            : '';
        let username = this.state.username
            ? this.state.username
            : '';

        let client: LoginClient = new LoginClient();
        client.login(username, password)
            .then((token: string) => {
                this.setState({ loading: false });
                let payload: JwtTokenPayload = this.jwtManager.save(token);
                this.props.setSession(payload);
            })
            .catch((error) => {
                this.setState({
                    errorText: error.response && typeof JSON.parse(error.response) === "string"
                        ? JSON.parse(error.response)
                        : error.response
                            ? error.response
                            : error.message ? error.message : error.toString(),
                    loading: false
                });
            });
    }

    register(e: any): void {
        this.setState({ redirectToRegister: true });
    }

    clearError(): void {
        this.setState({ errorText: '' });
    }

    render(): JSX.Element {

        if (this.state.redirectToRegister) {
            this.setState({ redirectToRegister: false });
            return this.renderRegisterRedirect();
        }

        if (this.jwtManager.validateToken()) {
            return this.renderLoggedIn();
        }
        else {
            return this.renderAnonymous();
        }
    }

    renderLoggedIn(): JSX.Element {
        return (<Redirect to={ExistingPages.home} push={true} />);
    }

    renderRegisterRedirect(): JSX.Element {
        return (<Redirect to={ExistingPages.register} push={true} />);
    }

    renderAnonymous(): JSX.Element {

        return (
            <div className={"loginContainer"}>
                <div className={"panelLogin"}>
                    <div>
                        <div className={"panelRowHeader"}>
                            <Typography className="title" variant={'h3' as const}>{'TimeZone manager'}</Typography>
                        </div>
                        <br />
                        <div className={"panelRow"}>
                            <TextField
                                autoFocus={true}
                                className={"inputControl"}
                                name={'username'}
                                label={'User'}
                                value={this.state.username}
                                onChange={this.handleChange}
                                onFocus={this.clearError}
                                autoComplete='username'
                            />
                        </div>
                        <div className={"panelRow"}>
                            <TextField
                                type={'password'}
                                className={"inputControl"}
                                name={'password'}
                                label={'Password'}
                                value={this.state.password}
                                onChange={this.handleChange}
                                onFocus={this.clearError}
                                autoComplete='current-password'
                            />
                        </div>
                        <div className={"panelRow"} onClick={this.submitLogin}>
                            <Button
                                className={"buttonControl"}
                                type={'submit'}
                                value={'Login'}
                                disabled={this.isNotReadyToLogin()}
                            />
                        </div>
                        <br />
                        <div className={"panelRowError"}>
                            <ul className="ul">
                                <li>{this.state.errors.username}</li>
                                <li>{this.state.errors.password}</li>
                            </ul>
                        </div>
                        <div className={"panelRowError"}>
                            <span>{this.state.errorText}</span>
                        </div>
                        <br />
                        <br />
                        <div className={"panelRow"} onClick={this.register}>
                            <Button
                                className={"buttonControl"}
                                type={'submit'}
                                value={'Register'}
                                disabled={this.state.loading}
                            />
                        </div>
                    </div>
                </div>
            </div>
        );
    }
}

export default connect(
    (state: ApplicationState) => state.session, // Selects which state properties are merged into the component's props
    SessionStore.actionCreators // Selects which action creators are merged into the component's props
)(Login as any);