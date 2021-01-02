import { UserAgentApplication } from 'msal';

export const LOGIN_SCOPES = ["openid", "profile", "user.read"];
export const API_SCOPES = ["api://a14ce592-08cf-4bdb-a104-f18606cb695e/access_user_data"]

export const acquireToken = () => {
    var userRequest = {
        scopes: API_SCOPES
    };

    try {
        return msalApp.acquireTokenSilent(userRequest);        
    } catch(error) {
        console.log("Error = ", error);
    }
}

export const fetchAPI = (url, accessToken) => {
    const response = fetch(url, {
        responseType: 'text',
        headers: {
            Authorization: `Bearer ${accessToken}`
        }
    });

    return response;
}

export const msalApp = new UserAgentApplication({
    auth: {
        clientId: '0ec444fe-05f8-4344-96cd-3186c4932325',
        authority: 'https://login.microsoftonline.com/514d5bdd-4518-4760-9408-f08191849cfa',
        validateAuthority: true,
        postLogoutRedirectUri: "http://localhost:3000",
        navigateToLoginRequestUrl: true
},
    cache: {
        cacheLocation: "sessionStorage",
        storeAuthStateInCookie: false
    }
})