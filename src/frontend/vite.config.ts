import { defineConfig } from 'vite';
import react from '@vitejs/plugin-react';

// https://vite.dev/config/
export default defineConfig({
  plugins: [react()],
  server: {
    proxy: {
      // Proxy API calls to the planner API service
      '/api': {
        target: process.env.PLANNER_API_HTTPS || process.env.PLANNER_API_HTTP,
        changeOrigin: true
      },
      // Proxy coordinator calls to the coordinator agent
      '/coordinator': {
        target: process.env.COORDINATOR_AGENT_HTTP,
        changeOrigin: true,
        rewrite: (path) => path.replace(/^\/coordinator/, '')
      }
    }
  }
});
