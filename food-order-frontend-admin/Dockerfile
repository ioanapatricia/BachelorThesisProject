FROM node:lts-alpine as builder
WORKDIR /usr/src/app
COPY . ./
RUN npm install
RUN npm run build
FROM nginx:alpine
COPY --from=builder /usr/src/app/dist /var/www
COPY nginx.conf /etc/nginx/nginx.conf
EXPOSE 3000
ENTRYPOINT ["nginx","-g","daemon off;"]