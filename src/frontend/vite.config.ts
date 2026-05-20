import { defineConfig } from 'vite';
import react from '@vitejs/plugin-react';

// https://vite.dev/config/
export default defineConfig({
  plugins: [react()],
  server: {
    proxy: {
      // Proxy API calls to the planner API service
      '/api': {
        target: process.env.SERVER_HTTPS || process.env.SERVER_HTTP,
        changeOrigin: true
      },
      // Proxy coordinator calls to the coordinator agent
      '/coordinator': {
        target: process.env.services__coordinator_agent__https__0 || process.env.services__coordinator_agent__http__0,
        changeOrigin: true,
        rewrite: (path) => path.replace(/^\/coordinator/, '')
      }
    }
  }
});
