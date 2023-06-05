import React, { useState } from 'react';
import axios from 'axios';
import { API } from '../../commons/Constants';
import { useNavigate } from 'react-router-dom';
import {Alert, Button, Col, Form} from "react-bootstrap";

const Register = () => {
    const [username, setUsername] = useState('');
    const [password, setPassword] = useState('');
    const [is_admin, setIsAdmin] = useState(false);
    const [errorMessage, setErrorMessage] = useState('');
    const [successMessage, setSuccessMessage] = useState('');
    const navigate = useNavigate();

    const handleInputChange = (e) => {
        const { name, value } = e.target;

        if (name === 'username') {
            setUsername(value);
        } else if (name === 'password') {
            setPassword(value);
        }
    };

    const handleCheckboxChange = (e) => {
        if (e.target.name === 'is_admin') {
            setIsAdmin(e.target.checked);
        }
    };

    const handleLogin = async (e) => {
        e.preventDefault();

        try {
            const response = await axios.post(`${API}/register`, {
                username: username,
                password: password,
                IsAdmin: is_admin,
            });
            if(response.status === 200){
                setSuccessMessage('Register successful!');
                setUsername('');
                setPassword('');
                setTimeout(() => {
                    setSuccessMessage('');
                    navigate('/login');
                }, 2000);
            }
            
        } catch (error) {
            setErrorMessage('Invalid username or password');
        }
    };

    return (
        <div>
            <h2>Register</h2>
            {errorMessage && (
                <Alert variant="danger" onClose={() => setErrorMessage('')} dismissible>
                    {errorMessage}
                </Alert>
            )}
            {successMessage && (
                <Alert variant="success" onClose={() => setSuccessMessage('')} dismissible>
                    {successMessage}
                </Alert>
            )}
            
            <Form onSubmit={handleLogin}>
                <Form.Group controlId="username">
                    <Form.Label>Username:</Form.Label>
                    <Col xs={12} sm={6} md={4} lg={3}>
                        <Form.Control
                            type="text"
                            name="username"
                            value={username}
                            onChange={handleInputChange}
                            required
                            size="md"
                        />
                    </Col>
                </Form.Group>
                <Form.Group controlId="password">
                    <Form.Label>Password:</Form.Label>
                    <Col xs={12} sm={6} md={4} lg={3}>
                        <Form.Control
                            type="password"
                            name="password"
                            value={password}
                            onChange={handleInputChange}
                            required
                            size="md"
                        />
                    </Col>
                </Form.Group>
                <Form.Group controlId="is_admin">
                    <Form.Check
                        type="checkbox"
                        name="is_admin"
                        label="Is admin"
                        defaultChecked={is_admin}
                        onChange={handleCheckboxChange}
                    />
                </Form.Group>
                <Button variant="primary" type="submit" className="my-2">Register</Button>
            </Form>
        </div>
    );
};

export default Register;
