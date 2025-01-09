import React, {act, useState, useEffect} from "react";
import LogoutButton from "../LogoutButton/LogoutButton";
import { useMsal } from "@azure/msal-react";
import ListUrls from "../ListUrls/ListUrls";
import axios from "axios";

function Home() {

    const scope = `api://${process.env.REACT_APP_CLIENT_ID}/Urls.Read`
    const apiEndpoint = process.env.REACT_APP_API_ENDPOINT;

    const { instance, accounts } = useMsal();
    const [data, setData] = useState({
        initialized: false,
        urls: []  
    });

    const handleLogout = () => {
        instance.logoutRedirect();
    }

    const getToken = async() => {
        const request = {
            scopes: [`openid profile ${scope}`],
            account: accounts[0]
        };
        
        const response = await instance.acquireTokenSilent(request);
        return response.accessToken;
    };

    const fetchUrls = async() => {
        var token = await getToken();
        const response = await axios.get(`${apiEndpoint}/api/urls`, {
            headers: { Authorization: `Bearer ${token}`}
        });

        setData({
            initialized: true,
            urls: response.data.urls
        });
    };

    useEffect(() => {
        if (!data.initialized)
            fetchUrls();
    });

    return (
        <div>
            Welcome!
            <LogoutButton onLogout={handleLogout}></LogoutButton>
            <ListUrls urls={data.urls}/>
        </div>
    );
}

export default Home;
