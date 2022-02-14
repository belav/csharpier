FROM node:14-alpine AS base
WORKDIR /app
EXPOSE 80

WORKDIR /src
COPY ./package.json .
COPY ./package-lock.json .

RUN npm ci

COPY . .
RUN npm run build

ENTRYPOINT npm run serve -- --build --port 80 --host 0.0.0.0
