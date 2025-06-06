import { defineStore } from "pinia";
import { fetchThemeSet } from "@/services/api";
import { decodeJwtToken } from "@/utils/jwt";

export const useAuthStore = defineStore("auth", {
  state: () => ({
    idUser: null,
    email: null,
    idThemeSet: null,
  }),
  actions: {
    initializeAuth() {
      const storedAuth = sessionStorage.getItem("auth");
      if (storedAuth) {
        const { idUser, email } = JSON.parse(storedAuth);
        this.idUser = idUser;
        this.email = email;
        return;
      }

      const decoded = decodeJwtToken();
      if (decoded) {
        this.idUser = decoded.idUser;
        this.email = decoded.email;

        this.saveToSessionStorage();
      }
    },

    saveToSessionStorage() {
      const authData = {
        idUser: this.idUser,
        email: this.email,
      };
      sessionStorage.setItem("auth", JSON.stringify(authData));
    },

    clearSessionStorage() {
      sessionStorage.removeItem("auth");
    },
  },
});


function hexToHSL(hex) {
  let r = parseInt(hex.slice(1, 3), 16) / 255;
  let g = parseInt(hex.slice(3, 5), 16) / 255;
  let b = parseInt(hex.slice(5, 7), 16) / 255;

  let cMin = Math.min(r, g, b),
    cMax = Math.max(r, g, b),
    delta = cMax - cMin,
    h = 0,
    s = 0,
    l = (cMax + cMin) / 2;

  if (delta) {
    if (cMax === r) {
      h = ((g - b) / delta) % 6;
    } else if (cMax === g) {
      h = (b - r) / delta + 2;
    } else {
      h = (r - g) / delta + 4;
    }

    s = delta / (1 - Math.abs(2 * l - 1));
  }

  return [h * 60, s * 100, l * 100];
}

function hslToHex(h, s, l) {
  s /= 100;
  l /= 100;

  let c = (1 - Math.abs(2 * l - 1)) * s,
    x = c * (1 - Math.abs(((h / 60) % 2) - 1)),
    m = l - c / 2,
    r = 0,
    g = 0,
    b = 0;

  if (0 <= h && h < 60) {
    r = c;
    g = x;
    b = 0;
  } else if (60 <= h && h < 120) {
    r = x;
    g = c;
    b = 0;
  } else if (120 <= h && h < 180) {
    r = 0;
    g = c;
    b = x;
  } else if (180 <= h && h < 240) {
    r = 0;
    g = x;
    b = c;
  } else if (240 <= h && h < 300) {
    r = x;
    g = 0;
    b = c;
  } else if (300 <= h && h < 360) {
    r = c;
    g = 0;
    b = x;
  }

  let red = Math.round((r + m) * 255)
      .toString(16)
      .padStart(2, "0"),
    green = Math.round((g + m) * 255)
      .toString(16)
      .padStart(2, "0"),
    blue = Math.round((b + m) * 255)
      .toString(16)
      .padStart(2, "0");

  return "#" + red + green + blue;
}

function autoAdjustLightness(hexColor, lightPercent = 5, darkPercent = -10) {
  const [h, s, l] = hexToHSL(hexColor);

  if (l > 70) {
    const newL = Math.max(0, Math.min(100, l + darkPercent));
    return hslToHex(h, s, newL);
  }
  else if (l < 30) {
    const newL = Math.max(0, Math.min(100, l + lightPercent));
    return hslToHex(h, s, newL);
  }
  else {
    return hexColor;
  }
}

export const useThemeStore = defineStore("theme", {
  state: () => ({
    idThemeSet: null,
    theme: null,
    importanceThemes: [],
  }),
  actions: {
    async loadTheme(themeId) {
      const authStore = useAuthStore();
      if (!authStore.idUser) {
        console.error("Пользователь не авторизован");
        return;
      }
      try {
        const themeSet = await fetchThemeSet(authStore.idUser, themeId);

        this.theme = themeSet.theme;
        this.importanceThemes = themeSet.importanceThemes;

        this.applyTheme();
        this.applyImportanceStyles();

        localStorage.setItem("theme", JSON.stringify(this.theme));
        localStorage.setItem(
          "importanceThemes",
          JSON.stringify(this.importanceThemes),
        );

        this.idThemeSet = themeSet.idThemeSet;
      } catch (error) {
        console.error("Не удалось загрузить тему:", error);
      }
    },

    loadThemeFromStorage() {
      const storedTheme = localStorage.getItem("theme");
      const storedImportanceThemes = localStorage.getItem("importanceThemes");

      if (storedTheme) {
        this.theme = JSON.parse(storedTheme);
        this.applyTheme();
      }

      if (storedImportanceThemes) {
        this.importanceThemes = JSON.parse(storedImportanceThemes);
        this.applyImportanceStyles();
      }
    },

    applyTheme() {
      if (!this.theme) return;

      const root = document.documentElement;
      const theme = this.theme;

      root.style.setProperty("--color-background", theme.backgroundColor);
      root.style.setProperty("--color-text", theme.textColor);
      root.style.setProperty("--color-heading", theme.textColor);
      root.style.setProperty("--color-border", theme.borderColor);
      root.style.setProperty("--color-background-soft", theme.cardBackground);
      root.style.setProperty("--color-button", theme.buttonColor);
      root.style.setProperty("--color-button-text", theme.buttonTextColor);
      root.style.setProperty("--color-accent", theme.accentColor);
      root.style.setProperty("--color-primary", theme.primaryColor);
      root.style.setProperty("--color-secondary", theme.secondaryColor);
      root.style.setProperty(
        "--color-background-mute",
        autoAdjustLightness(theme.cardBackground, 5),
      );
      root.style.setProperty("--color-shadow", theme.shadowColor);
    },

    applyImportanceStyles() {
      const root = document.documentElement;

      this.importanceThemes.forEach((importance, index) => {
        const prefix = `--importance-${importance.idImportance}`;
        root.style.setProperty(`${prefix}-bg`, importance.backgroundColor);
        root.style.setProperty(`${prefix}-text`, importance.textColor);
      });
    },
  },
});
