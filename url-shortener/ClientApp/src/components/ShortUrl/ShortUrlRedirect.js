import {useParams} from "react-router-dom";
import React, {useState, useEffect} from "react";
import axios from "axios";
import {API} from "../../commons/Constants";

const ShortUrlRedirect = () => {
    const {id} = useParams();
    const [error, setError] = useState(false);

    useEffect(() => {
        const getLongUrl = async () => {
            try{
                const response = await axios.get(`${API}/shorturl/${id}`);
                window.location.replace(response.data.longUrl)
            }
            catch (e){
                setError(true)
            }
        }
        getLongUrl();
    }, [])
    
    if(error)
        return (
            <div>
                <h1>Unable to redirect</h1>
            </div>
        )
    return (
        <div>
            <h1>Redirecting..</h1>
        </div>
    )
}

export default ShortUrlRedirect