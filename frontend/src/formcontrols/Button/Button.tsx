import * as React from 'react';
import MuiButton, { ButtonProps as MuiButtonProps } from '@material-ui/core/Button';

import './Button.css';

type VariantType = 'text' | 'outlined' | 'contained';
type ColorType = 'inherit' | 'primary' | 'secondary' | 'default';

interface ButtonProps extends React.HTMLProps<HTMLButtonElement> {
    variant?: VariantType;
    color?: ColorType;
    icon?: JSX.Element;
    disabled?: boolean;

    // Workaround for wrapping an <input> in a MuiButton:
    // https://github.com/mui-org/material-ui/issues/647#issuecomment-244917962
    component?: React.ReactType<MuiButtonProps>;
}

type ButtonDefaultProps = 'variant' | 'color' | 'className' | 'disabled';

export class Button extends React.Component<ButtonProps> {

    static defaultProps: Pick<ButtonProps, ButtonDefaultProps> = { disabled: false, variant: 'contained', color: 'secondary', className: '' };

    render(): JSX.Element {
        let { value } = this.props;
        return (
            <MuiButton
                variant={this.props.variant}
                className={this.props.className}
                color={this.props.color}
                disabled={this.props.disabled}
            >
                {value ? value : this.props.children}
                {this.renderIcon()}
            </MuiButton>
        );
    }

    renderIcon(): JSX.Element {
        let content: JSX.Element | null = null;
        if (this.props.icon) {
            content = (
                <div className={"icon"}>
                    {this.props.icon}
                </div>
            );
        }
        return content ? content : <div></div>;
    }
}

