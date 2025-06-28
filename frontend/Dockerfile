# Etapa de build
FROM node:20-alpine AS build
WORKDIR /app
COPY package.json package-lock.json* pnpm-lock.yaml* yarn.lock* ./
RUN npm install
COPY . .

# Definir a variável de ambiente durante o build
ARG VITE_API_URL=http://localhost:5097/api
ENV VITE_API_URL=$VITE_API_URL

RUN npm run build

# Etapa de produção
FROM nginx:alpine
WORKDIR /usr/share/nginx/html
COPY --from=build /app/dist .
COPY ./nginx.conf /etc/nginx/conf.d/default.conf
EXPOSE 80
CMD ["nginx", "-g", "daemon off;"]