(function (window) {
    const configAccessor = "__config__";
    window[configAccessor] = window[configAccessor] || {};

    // Environment variables
    const env = {
        production: false,
        api: {
            url: "${API_URL}"
        },
        identity: {
            url: "${IDENTITY_URL}",
            clientId: "${IDENTITY_CLIENT_ID}",
            realm: "${IDENTITY_REALM}",
        }
    };

    window[configAccessor] = env;
})(this);
