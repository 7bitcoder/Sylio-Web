(function (window) {
    const configAccessor = "__config__";
    window[configAccessor] = window[configAccessor] || {};

    // Environment variables
    const env = {
        production: false,
        api: {
            url: "http://localhost:5200",
        },
        identity: {
            url: "http://localhost:8080",
            clientId: "frontend",
            realm: "sylio"
        }
    };

    window[configAccessor] = env;
})(this);
