import React, { Component } from 'react';
import { useNavigate } from 'react-router-dom';
import axios from 'axios';
import {API} from '../../commons/Constants'
import decodeJWT from "jwt-decode"

export class Login extends Component {
    static displayName = Login.name;
    constructor(props) {
        super(props);
        this.state = {
            username: '',
            password: '',
            errorMessage: ''
        };
    }

    handleInputChange = (e) => {
        this.setState({ [e.target.name]: e.target.value });
    };

    handleLogin = async (e) => {
        e.preventDefault();

        try {
            const response = await axios.post(API + 'login', {
                username: this.state.username,
                password: this.state.password
            });
            
            localStorage.setItem('userName', this.state.username);
            
            // Redirect after successful login
            const navigate = useNavigate();
            navigate('/home');
            
        } catch (error) {
            this.setState({ errorMessage: 'Invalid username or password ' + error});
        }
    };

    render() {
        const { username, password, errorMessage } = this.state;

        return (
            <div>
                <h2>Login</h2>
                {errorMessage && <div>{errorMessage}</div>}
                <form onSubmit={this.handleLogin}>
                    <div>
                        <label>Username:</label>
                        <input
                            type="text"
                            name="username"
                            value={username}
                            onChange={this.handleInputChange}
                            required
                        />
                    </div>
                    <div>
                        <label>Password:</label>
                        <input
                            type="password"
                            name="password"
                            value={password}
                            onChange={this.handleInputChange}
                            required
                        />
                    </div>
                    <button type="submit">Login</button>
                </form>
            </div>
        );
    }
}
