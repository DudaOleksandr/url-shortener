import decodeJWT from 'jwt-decode';

export const getUserRole = () => {
    const token = localStorage.getItem('token');
    if (token) {
        const decodedToken = decodeJWT(token);
        return decodedToken['http://schemas.microsoft.com/ws/2008/06/identity/claims/role'];
    }
    return null;
};