import React, { useState, useEffect } from 'react';
import axios from 'axios';
import { API } from '../../commons/Constants';
import {Table, Button, Form, InputGroup, Alert, Row, Col} from 'react-bootstrap';

const ShortUrls = () => {
    const [shortUrls, setShortUrls] = useState([]);
    const [newUrl, setNewUrl] = useState('');
    const [error, setError] = useState('');

    useEffect(() => {
        fetchShortUrls();
    }, []);

    const fetchShortUrls = async () => {
        try {
            const response = await axios.get(`${API}/shorturl`);
            setShortUrls(response.data);
        } catch (error) {
            setShortUrls([]);
            setError(`Error fetching URLs`);
            
        }
    };

    const handleDelete = async (id) => {
        try {
            await axios.delete(`${API}/shorturl/${id}`);
            await fetchShortUrls();
        } catch (error) {
            setError(`Error deleting URL: ${id}`);
        }
    };

    const handleDeleteAll = async () => {
        try {
            await axios.delete(`${API}/shorturl`);
            await fetchShortUrls();
        } catch (error) {
            setError('Error deleting all short URLs');
        }
    };

    const handleAddUrl = async () => {
        try {
            const response = await axios.post(`${API}/shorturl`, {
                originalUrl: newUrl});
            setNewUrl('');
            setError('');
            await fetchShortUrls();
        } catch (error) {
            setError('Error adding URL');
        }
    };

    return (
        <div>
            {error && (
                <Alert variant="danger" onClose={() => setError('')} dismissible>
                    {error}
                </Alert>
            )}

            <div className="mb-5">
                <h2>Add new URL</h2>
                <InputGroup>
                    <Form.Control type="text" value={newUrl} onChange={(e) => 
                        setNewUrl(e.target.value)} />
                    <Button onClick={handleAddUrl}>Add</Button>
                </InputGroup>
            </div>

            <Row className="mb-3">
                <Col>
                    <h2>Short URLs Table</h2>
                </Col>
                {shortUrls.length > 0 && (
                    <Col className="d-flex justify-content-end">
                        <Button variant="danger" onClick={handleDeleteAll}>
                            Delete All
                        </Button>
                    </Col>
                )}
            </Row>

            <Table striped bordered hover>
                <thead>
                <tr>
                    <th>Original URL</th>
                    <th>Short URL</th>
                    <th>Info</th>
                    <th>Actions</th>
                </tr>
                </thead>
                <tbody>
                {shortUrls.map((shortUrl) => (
                    <tr key={shortUrl.id}>
                        <td>{shortUrl.longUrl}</td>
                        <td>
                            <a href={window.location.origin + '/redirect/' + shortUrl.shortUrl}>
                                {window.location.origin + '/redirect/' + shortUrl.shortUrl}
                            </a>
                        </td>
                        <td>
                            <a href={window.location.origin + '/info/' + shortUrl.shortUrl}>
                                {shortUrl.shortUrl}
                            </a>
                        </td>
                        <td>
                            <Button variant="danger" onClick={() => handleDelete(shortUrl.id)}>
                                Delete
                            </Button>
                        </td>
                    </tr>
                ))}
                </tbody>
            </Table>
        </div>
    );
};

export default ShortUrls;
