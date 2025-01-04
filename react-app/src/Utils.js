import { jwtDecode } from 'jwt-decode';


export const getUserRole = (token) => {
  try {
    // const decodedToken = jwt_decode(token);
    const decodedToken = jwtDecode(token);
    return decodedToken.role || decodedToken["http://schemas.microsoft.com/ws/2008/06/identity/claims/role"];
  } catch (error) {
    console.error("Failed to decode token:", error);
    return null;
  }
};
