export function useAuthFetch<T>(url: string, options: any = {}) {
  const token = localStorage.getItem("token");

  const headers = {
    ...options.headers,
    Authorization: token ? `Bearer ${token}` : undefined,
  };

  return useFetch<T>(url, { ...options, headers });
}
