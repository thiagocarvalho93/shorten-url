<template>
  <div class="flex h-screen bg-white dark:bg-gray-900 text-gray-800 dark:text-white">
    <!-- Sidebar -->
    <aside class="w-64 bg-gray-100 dark:bg-gray-800 p-6 flex flex-col justify-between">
      <nav class="space-y-4">
        <NuxtLink
          to="/"
          class="block px-4 py-2 rounded-md hover:bg-gray-200 dark:hover:bg-gray-700"
          active-class="font-bold bg-gray-200 dark:bg-gray-700"
        >
          🏠 Home
        </NuxtLink>
        <NuxtLink
          to="/links"
          class="block px-4 py-2 rounded-md hover:bg-gray-200 dark:hover:bg-gray-700"
          active-class="font-bold bg-gray-200 dark:bg-gray-700"
        >
          🔗 Links
        </NuxtLink>
      </nav>

      <div class="space-y-4">
        <BaseDarkModeToggle />

        <button
          @click="handleLogout"
          class="w-full text-sm px-4 py-2 text-red-700 border border-red-700 rounded-md hover:bg-red-100 dark:text-red-400 dark:border-red-400 dark:hover:bg-red-900/20"
        >
          Logout
        </button>
      </div>
    </aside>

    <!-- Main content area -->
    <main class="flex-1 overflow-y-auto p-8">
      <slot />
    </main>
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
