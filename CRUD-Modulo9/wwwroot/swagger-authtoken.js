(() => {
    const originalFetch = window.fetch.bind(window);

    window.fetch = async (input, init) => {
        const url = new URL(typeof input === "string" ? input : input.url, window.location.href);
        const method = (init?.method || input.method || "GET").toUpperCase();
        const response = await originalFetch(input, init);

        if (method === "POST" && url.pathname.endsWith("/api/auth/login") && response.ok) {
            response.clone().json()
                .then(data => {
                    if (typeof data.token === "string" && data.token.length > 0) {
                        window.ui.authActions.authorize({
                            Bearer: {
                                name: "Bearer",
                                schema: { type: "http", scheme: "bearer", bearerFormat: "JWT" },
                                value: data.token
                            }
                        });
                    }
                })
                .catch(error => console.error("No se pudo leer el token de autenticación.", error));
        }

        return response;
    };
})();
