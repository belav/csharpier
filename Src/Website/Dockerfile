FROM node:14-alpine AS base
EXPOSE 80

WORKDIR /src
COPY ./package.json .
COPY ./package-lock.json .

RUN npm ci

COPY . .
RUN npm run build

ENTRYPOINT npm run serve -- --port 80 --host 0.0.0.0
