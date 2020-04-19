export const APP_PERMISSIONS = {
    "query:permission:any": "query:permission:any",
    "query:roles:any": "query:roles:any",
};

export type AppPermission = keyof typeof APP_PERMISSIONS;
