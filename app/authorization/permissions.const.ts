export const APP_PERMISSIONS = {
    "role.create": "role.create",
    "role.read": "role.read",
    "role.update": "role.update",
    "role.delete": "role.delete",
    "permission.create": "permission.create",
    "permission.read": "permission.read",
    "permission.update": "permission.update",
    "permission.delete": "permission.delete",
};

export type AppPermission = keyof typeof APP_PERMISSIONS;
