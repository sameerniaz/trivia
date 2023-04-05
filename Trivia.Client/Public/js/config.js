// Config service

const configService = {
    isInit: false,
    value: {
        userBaseUrl: null
    },
    async fetch() {
        if (this.isInit) {
            return this.value;
        }

        console.log("Fetching application configuration.");

        const response = await fetch("/config");
        if (!response.ok) {
            console.error("A network error occurred while fetching application configuration.");
        }
        this.value = await response.json();

        console.log("Application configuration: ", this.value);

        return this.value;
    }
};