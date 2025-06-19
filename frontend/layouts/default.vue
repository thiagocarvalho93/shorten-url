<template>
  <div class="isolate h-screen p-6 lg:px-8">
    <div class="flex justify-end gap-3">
      <button
        @click="handleLogout"
        class="text-sm px-4 py-2 text-red-700 border border-red-700 rounded-md hover:bg-red-50 dark:text-red-400 dark:border-red-400 dark:hover:bg-red-900/20"
      >
        Logout
      </button>
      <BaseDarkModeToggle />
    </div>
    <slot />
  </div>
</template>

<script setup>
import { useRouter } from "vue-router";

const router = useRouter();

const handleLogout = () => {
  localStorage.removeItem("token");
  router.push("/login");
};
</script>
