import React, { useState } from 'react';
import { Form } from 'react-bootstrap';

const About = () => {
    const [description, setDescription] = useState('Hashids is a small open-source library that generates short, unique, non-sequential ids from numbers.\n' +
        '\n' +
        'It converts numbers like 347 into strings like “yr8”, or array of numbers like [27, 986] into “3kTMd”.\n' +
        '\n' +
        'You can also decode those ids back. This is useful in bundling several parameters into one or simply using them as short UIDs.'
    );
    

    return (
        <div className="about-container">
            <h2>About</h2>
            
            <Form>
                <Form.Group controlId="description">
                    <Form.Label>Description</Form.Label>
                    <Form.Control
                        as="textarea"
                        rows={5}
                        value={description}
                        onChange={(e) => setDescription(e.target.value)}
                    />
                </Form.Group>
            </Form>
        </div>
    );
};

export default About;
