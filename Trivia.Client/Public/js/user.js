// User Service

const userService = {
    isInit: false,
    baseUrl: null,
    token: null,
    async init() {
        if (this.isInit) {
            return;
        }
        
        console.log("Initializing user service.");

        const config = await configService.fetch();
        this.baseUrl = config.userBaseUrl;
        
        this.isInit = true;
        
        console.log("User service ready.")
    },
    async login(username, password) {
        console.log(`Attempting login as ${username}.`);
        
        await this.init();
    }
};