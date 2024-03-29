import * as React from 'react';
import { ApplicationState } from '../store';
import { SessionSelector } from '../selectors/sessionSelector';
import { PermissionName } from '../server/autogeneratedclients/TimeZoneManagerClient'
import { connect } from 'react-redux';
import { Redirect } from 'react-router';

interface AuthorizedComponentProps {
    requiredPermissions: PermissionName[];
    component: JSX.Element;
}

interface AuthorizedComponentStoreProps {
    hasPermission: boolean;
}

type AuthorizedComponentAllProps = AuthorizedComponentProps & AuthorizedComponentStoreProps;

class AuthorizedComponentInternal extends React.Component<AuthorizedComponentAllProps> {

    public render(): JSX.Element {
        if (this.props.hasPermission) {
            return (this.props.component);
        }
        else {
            return <Redirect to='/' />
        }
    }
}

function mapStateToProps(state: ApplicationState, props: AuthorizedComponentProps): AuthorizedComponentStoreProps {
    return { hasPermission: SessionSelector.hasPermissions(props.requiredPermissions)(state) };
}

export const AuthorizedComponent = connect(mapStateToProps)(AuthorizedComponentInternal);
