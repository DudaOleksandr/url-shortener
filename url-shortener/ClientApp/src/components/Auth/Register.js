import React, { Component } from 'react';
import axios from 'axios';
import {API} from "../../commons/Constants";

export class Register extends Component {
    static displayName = Register.name;
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
            const response = await axios.post(API + 'register', {
                username: this.state.username,
                password: this.state.password
            });

            // Assuming the server returns a token upon successful login
            const token = response.data.token;

            // Store the token in localStorage or any other desired method
            localStorage.setItem('token', token);

            // Redirect or perform any other actions after successful login
        } catch (error) {
            this.setState({ errorMessage: 'Invalid username or password ' + error });
        }
    };

    render() {
        const { username, password, errorMessage } = this.state;

        return (
            <div>
                <h2>Register</h2>
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
                    <button type="submit">Register</button>
                </form>
            </div>
        );
    }
}
