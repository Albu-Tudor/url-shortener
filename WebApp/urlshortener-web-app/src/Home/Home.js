import React from "react";
import LogoutButton from "../LogoutButton/LogoutButton";
import { useMsal } from "@azure/msal-react";

function Home() {
    const { instance } = useMsal();
    
    const handleLogout = () => {
        instance.logoutRedirect();
    }

    return (
        <div>
            Welcome!
            <LogoutButton onLogout={handleLogout}></LogoutButton>
        </div>
    );
}

export default Home;
