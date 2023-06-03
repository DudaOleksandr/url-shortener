import React, { Component } from 'react';
import axios from 'axios';
import {API} from "../../commons/Constants";
import { Table, Button, Form, InputGroup  } from 'react-bootstrap';

export class ShortUrls extends Component {
    constructor(props) {
        super(props);
        this.state = {
            shortUrls: [],
            newUrl: '',
            error: '',
        };
    }

    componentDidMount() {
        this.fetchShortUrls();
    }

    fetchShortUrls = async () => {
        try {
            const response = await axios.get(API + 'shorturl');
            this.setState({ shortUrls: response.data });
        } catch (error) {
            this.setState({ shortUrls: [] });
            console.error('Error fetching short URLs:', error);
        }
    };

    handleDelete = async (id) => {
        try {
            await axios.delete(API + `shorturl/${id}`);
            await this.fetchShortUrls();
        } catch (error) {
            console.error('Error deleting short URL:', error);
        }
    };

    handleAddUrl = async () => {
        try {
            const { newUrl } = this.state;
            /*TODO Add created by*/
            const response = await axios.post(API + 'shorturl', { originalUrl: newUrl, creatorName: "Test" });
            this.setState({ newUrl: '', error: '' });
            await this.fetchShortUrls();
        } catch (error) {
            if (error.response && error.response.data && error.response.data.message) {
                this.setState({ error: error.response.data.message });
            } else {
                console.error('Error adding short URL:', error);
            }
        }
    };

    render() {
        const { shortUrls, newUrl, error } = this.state;

        return (
            <div>
                <h1>Short URLs Table</h1>

                {error && <div>{error}</div>}

                <Table striped bordered hover>
                    <thead>
                    <tr>
                        <th>Original URL</th>
                        <th>Short URL</th>
                        <th>Created By</th>
                        <th>Actions</th>
                    </tr>
                    </thead>
                    <tbody>
                    {shortUrls.map((shortUrl) => (
                        <tr key={shortUrl.id}>
                            <td>{shortUrl.longUrl}</td>
                            <td>
                                <a href={window.location.origin +'/redirect/' + shortUrl.shortUrl}>{shortUrl.shortUrl}</a>
                            </td>
                            <td>{shortUrl.createdBy}</td>
                            <td>
                                <Button variant="danger" onClick={() => this.handleDelete(shortUrl.id)}>
                                    Delete
                                </Button>
                            </td>
                        </tr>
                    ))}
                    </tbody>
                </Table>

                <div>
                    <h2>Add new URL</h2>
                    <InputGroup>
                        <Form.Control
                            type="text"
                            value={newUrl}
                            onChange={(e) => this.setState({ newUrl: e.target.value })}
                        />
                        <Button onClick={this.handleAddUrl}>Add</Button>
                    </InputGroup>
                </div>
            </div>
        );
    }
}
