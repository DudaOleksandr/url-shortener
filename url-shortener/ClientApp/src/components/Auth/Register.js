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
            is_admin: false,
            errorMessage: ''
        };
    }

    handleInputChange = (e) => {
        this.setState({ [e.target.name]: e.target.value });
    };

    handleCheckboxChange = (e) => {
        this.setState({ [e.target.name]: e.target.checked });
    };

    handleRegister = async (e) => {
        e.preventDefault();

        try {
            
            const response = await axios.post(API + 'register', {
                username: this.state.username,
                password: this.state.password,
                isAdmin: this.state.is_admin
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
        const { username, password, is_admin, errorMessage } = this.state;

        return (
            <div>
                <h2>Register</h2>
                {errorMessage && <div>{errorMessage}</div>}
                <form onSubmit={this.handleRegister}>
                    <div className="container-sm">
                        <label className="p-2">Username:</label>
                        <input
                            type="text"
                            name="username"
                            value={username}
                            onChange={this.handleInputChange}
                            required/>
                    </div>
                    <div className="container-sm">
                        <label className="p-2">Password:</label>
                        <input
                            type="password"
                            name="password"
                            value={password}
                            onChange={this.handleInputChange}
                            required/>
                    </div>
                    <div className="container-sm">
                        <label className="p-2">Is admin:</label>
                        <input
                            type="checkbox"
                            name="is_admin"
                            defaultChecked={is_admin}
                            onChange={this.handleCheckboxChange}/>
                    </div>
                    <button type="submit">Register</button>
                </form>
            </div>
        );
    }
}
