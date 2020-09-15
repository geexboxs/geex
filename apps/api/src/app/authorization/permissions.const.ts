
export const APP_PERMISSIONS = [
    "permission:query:any",
    "role:query:any",
  ] as const;

export type AppPermission = (typeof APP_PERMISSIONS)[number];
