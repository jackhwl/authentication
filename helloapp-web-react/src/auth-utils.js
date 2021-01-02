import { UserAgentApplication } from 'msal';

export const LOGIN_SCOPES = ["openid", "profile", "user.read"];

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