FROM node
COPY . /app
WORKDIR /app
RUN npm install
EXPOSE 8080
ENTRYPOINT ["npm", "run", "serve"]