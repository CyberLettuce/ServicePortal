window.settingsHelper = {
    setTheme: function (theme, darkMode) {
        const root = document.documentElement;
        root.classList.remove("dark-mode", "theme-sgs-light", "theme-slate-dark", "theme-indigo-light", "theme-midnight-blue", "theme-warm-neutral", "theme-rose-light");

        const selectedTheme = theme || (darkMode ? "SlateDark" : "SgsLight");
        const themeClass = {
            SgsLight: "theme-sgs-light",
            SlateDark: "theme-slate-dark",
            IndigoLight: "theme-indigo-light",
            MidnightBlue: "theme-midnight-blue",
            WarmNeutral: "theme-warm-neutral",
            RoseLight: "theme-rose-light"
        }[selectedTheme] || "theme-sgs-light";

        root.classList.add(themeClass);
        if (selectedTheme === "SlateDark" || selectedTheme === "MidnightBlue" || (!theme && darkMode)) {
            root.classList.add("dark-mode");
        }
    },
    setDarkMode: function (enabled) {
        if (enabled) {
            document.documentElement.classList.add("dark-mode");
        } else {
            document.documentElement.classList.remove("dark-mode");
        }
    }
};
