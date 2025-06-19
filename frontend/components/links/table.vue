<template>
  <table class="w-full table-fixed border border-gray-300 dark:border-gray-600 rounded-md">
    <thead class="bg-gray-100 dark:bg-gray-700 text-gray-900 dark:text-white">
      <tr>
        <th class="p-4 text-left w-1/2">Original URL</th>
        <th class="p-4 text-left w-1/3">Short URL</th>
        <th class="p-4 text-center w-1/6">Actions</th>
      </tr>
    </thead>
    <tbody class="bg-white dark:bg-gray-800 text-gray-700 dark:text-gray-200">
      <tr
        v-for="link in links"
        :key="link.token"
        class="border-t border-gray-200 dark:border-gray-600 hover:bg-gray-50 dark:hover:bg-gray-700 transition"
      >
        <td class="p-4 truncate" :title="link.originalUrl">
          {{ link.originalUrl }}
        </td>

        <td class="p-4 truncate" :title="shortUrl(link.shortCode)">
          <a
            :href="shortUrl(link.shortCode)"
            target="_blank"
            class="text-blue-600 dark:text-blue-400 hover:underline"
          >
            {{ shortUrl(link.shortCode) }}
          </a>
        </td>

        <td class="p-4 text-center">
          <button
            @click="copyShortUrl(link.shortCode)"
            class="px-3 py-1 text-sm bg-blue-600 hover:bg-blue-700 text-white rounded-md"
          >
            Copy
          </button>
        </td>
      </tr>
    </tbody>
  </table>
</template>

<script setup>
import { API_BASE_URL } from '~/constants';

const props = defineProps({
  links: {
    type: Array,
    required: true,
  },
});

const emit = defineEmits(["copy"]);

const shortUrl = (code) => {
  return `${API_BASE_URL}/${code}`;
};

const copyShortUrl = async (code) => {
  try {
    await navigator.clipboard.writeText(shortUrl(code));
    emit("copy", code);
  } catch {
    alert("Failed to copy.");
  }
};
</script>
