import React, { useState, useEffect } from 'react';
import {useParams} from "react-router-dom";

import axios from 'axios';
import { API } from '../../commons/Constants';
import { Table, Alert } from 'react-bootstrap';

const Info = () => {
    const {id} = useParams();
    const [shortUrl, setShortUrl] = useState([]);
    const [error, setError] = useState('');

    useEffect(() => {
        console.log(id);
        const fetchUrl = async () => {
            try {
                console.log(id);
                const response = await axios.get(`${API}/shorturl/${id}`);
                setShortUrl(response.data);
            } catch (e) {
                setShortUrl(undefined);
                setError([]);
                console.error('Error fetching short URL\n', e);
            }
        }
        fetchUrl();
    }, []);

    const formatDate = (dateString) => {
        const dateObject = new Date(dateString);
        const formattedDate = `${dateObject.getFullYear()}-${(
            dateObject.getMonth() + 1
        )
            .toString()
            .padStart(2, '0')}-${dateObject.getDate().toString().padStart(2, '0')}`;
        const formattedTime = `${dateObject
            .getHours()
            .toString()
            .padStart(2, '0')}:${dateObject
            .getMinutes()
            .toString()
            .padStart(2, '0')}:${dateObject.getSeconds().toString().padStart(2, '0')}`;
        return `${formattedDate} ${formattedTime}`;
    };
    
    return (
        <div>
            <h1>Short URL Info</h1>

            {error && (
                <Alert variant="danger" onClose={() => setError('')} dismissible>
                    {error}
                </Alert>
            )}

            <Table striped bordered hover>
                <thead>
                <tr>
                    <th>Original URL</th>
                    <th>Short URL</th>
                    <th>Created by</th>
                    <th>Creation date</th>
                </tr>
                </thead>
                <tbody>
                <tr>
                    <td>{shortUrl.longUrl}</td>
                    <td>
                        <a href={window.location.origin + '/redirect/' + shortUrl.shortUrl}>
                            {window.location.origin + '/redirect/' + shortUrl.shortUrl}
                        </a>
                    </td>
                    <td>
                        {shortUrl.createdBy}
                    </td>
                    <td>
                        {formatDate(shortUrl.createdDate)}
                    </td>
                </tr>
                </tbody>
            </Table>
        </div>
    );
};

export default Info;
