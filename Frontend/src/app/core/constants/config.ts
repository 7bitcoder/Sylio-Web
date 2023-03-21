const win = window as any;
export const config = win['__config__'] as {
    production: boolean,
    api: {
        url: string,
    },
    identity: {
        url: string,
        clientId: string,
        realm: string
    }
};
