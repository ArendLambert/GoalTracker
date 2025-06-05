import axios from "axios";

const apiClient = axios.create({
  baseURL: "http://localhost:5210",
  withCredentials: true,
  headers: {
    "Content-Type": "application/json",
  },
});

// ==== Theme ====
export const fetchThemeSet = async (userId, themeId) => {
  try {
    const response = await apiClient.get(`/Theme/getbyid/${userId}/${themeId}`);

    return response.data;
  } catch (error) {
    console.error(
      "Ошибка при загрузке темы:",
      error.response?.data || error.message,
    );
    throw error;
  }
};

export const fetchCreateTheme = async (theme, idUser) => {
  try {
    const themeData = {
      idUserCreator: theme.idUser,
      importanceThemes: theme.importanceThemes,
      id: theme.id,
      theme: theme.theme,
      public: false,
      requestUserId: idUser,
    };
    console.log("Создание темы:", themeData);
    const response = await apiClient.post("/Theme/createtheme", themeData);

    return response.data;
  } catch (error) {
    console.error(
      "Ошибка при создании темы:",
      error.response?.data || error.message,
    );
    throw error;
  }
};

export const fetchUpdateTheme = async (theme, idUser) => {
  try {
    const themeData = {
      idUserCreator: theme.idUser,
      importanceThemes: theme.importanceThemes,
      id: theme.id,
      theme: theme.theme,
      public: false,
      requestUserId: idUser,
    };
    console.log("Обновление темы:", themeData);
    const response = await apiClient.put("/Theme/update", themeData);

    return response.data;
  } catch (error) {
    console.error(
      "Ошибка при обновлении темы:",
      error.response?.data || error.message,
    );
    throw error;
  }
};

export const fetchThemes = async (userId) => {
  try {
    const response = await apiClient.get(`/Theme/${userId}`);

    return response.data;
  } catch (error) {
    console.error(
      "Ошибка при получении тем пользователя:",
      error.response?.data || error.message,
    );
    throw error;
  }
};

export const fetchDeleteTheme = async (userId, themeId) => {
  try {
    const response = await apiClient.delete(
      `/Theme/delete/${userId}/${themeId}`,
    );

    return response.data;
  } catch (error) {
    console.error(
      "Ошибка при удалении темы:",
      error.response?.data || error.message,
    );
    throw error;
  }
};

// ==== User ====
export const fetchCreateUser = async (email, password) => {
  try {
    const response = await apiClient.post("/User/register", {
      email,
      password,
    });

    return response.data;
  } catch (error) {
    console.error(
      "Ошибка при создании пользователя:",
      error.response?.data || error.message,
    );
    throw error;
  }
};

export const fetchExistUserByEmail = async (email) => {
  try {
    const response = await apiClient.get(`/User/existsbyemail/${email}`);

    return response.data;
  } catch (error) {
    console.error(
      "Ошибка при проверке пользователя:",
      error.response?.data || error.message,
    );
    throw error;
  }
};

export const fetchLoginUser = async (email, password) => {
  try {
    const response = await apiClient.post(`/User/login`, {
      email,
      password,
    });

    return response.data;
  } catch (error) {
    console.error(
      "Ошибка при входе пользователя:",
      error.response?.data || error.message,
    );
    throw error;
  }
};

export const fetchUpdateUser = async (id, email, password, idThemeSet) => {
  try {
    const response = await apiClient.put(`/User/update`, {
      id,
      email,
      password,
      idThemeSet,
    });

    return response.data;
  } catch (error) {
    console.error(
      "Ошибка при обновлении пользователя:",
      error.response?.data || error.message,
    );
    throw error;
  }
};

export const fetchGetUserById = async (id) => {
  try {
    const response = await apiClient.get(`/User/${id}`);

    return response.data;
  } catch (error) {
    console.error(
      "Ошибка при получении пользователя по ID:",
      error.response?.data || error.message,
    );
    throw error;
  }
};

// ==== Goal ====
export const fetchUserGoals = async (userId) => {
  try {
    const response = await apiClient.get(`/Goal/allforuser/${userId}`);

    return response.data;
  } catch (error) {
    console.error(
      "Ошибка при получении целей пользователя:",
      error.response?.data || error.message,
    );
    throw error;
  }
};

export const fetchGetGoalById = async (userId, goalId) => {
  try {
    const response = await apiClient.get(`/Goal/${userId}/${goalId}`);

    return response.data;
  } catch (error) {
    console.error(
      "Ошибка при получении задачи:",
      error.response?.data || error.message,
    );
    throw error;
  }
};

export const fetchCreateGoal = async (goal) => {
  try {
    // if(goal.idImportance = 'null') {
    //   goal.idImportance = null;
    // }
    console.log("Добавление цели:", goal);
    const response = await apiClient.post("/Goal/add", goal, {});

    return response.data;
  } catch (error) {
    console.error(
      "Ошибка при добавлении задачи:",
      error.response?.data || error.message,
    );
    throw error;
  }
};

export const fetchUpdateGoal = async (goal) => {
  try {
    console.log("Обновление цели:", goal);
    const response = await apiClient.put(`/Goal/update/${goal.idUser}`, {
      id: goal.id,
      title: goal.title,
      description: goal.description,
      idImportance: goal.idImportance,
      idStatus: goal.idStatus,
      deadline: goal.deadline,
      startDate: goal.startDate,
      punishment: goal.punishment,
      autoImportance: goal.autoImportance,
      idUser: goal.idUser,
      sendEmail: goal.sendEmail,
    });
    return response.data;
  } catch (error) {
    console.error(
      "Ошибка при обновлении задачи:",
      error.response?.data || error.message,
    );
    throw error;
  }
};

export const fetchDeleteGoal = async (userId, goalId) => {
  try {
    const response = await apiClient.delete(`/Goal/delete/${userId}/${goalId}`);

    return response.data;
  } catch (error) {
    console.error(
      "Ошибка при удалении задачи:",
      error.response?.data || error.message,
    );
    throw error;
  }
};

export const fetchImportances = async () => {
  try {
    const response = await apiClient.get("/Importance");

    return response.data;
  } catch (error) {
    console.error(
      "Ошибка при получении важностей:",
      error.response?.data || error.message,
    );
    throw error;
  }
};

export const fetchStatuses = async () => {
  try {
    const response = await apiClient.get("/Status");

    return response.data;
  } catch (error) {
    console.error(
      "Ошибка при получении статусов:",
      error.response?.data || error.message,
    );
    throw error;
  }
};
