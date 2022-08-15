FROM nginx:alpine
## Remove default nginx website
RUN apk add --no-cache tzdata
ENV TZ=Brazil/East
RUN ln -snf /usr/share/zoneinfo/$TZ /etc/localtime && echo $TZ > /etc/timezone
RUN rm -rf /usr/share/nginx/html/*
## From 'builder' stage copy over the artifacts in dist folder to default nginx public folder
COPY /dist /usr/share/nginx/html
RUN mkdir -p /usr/share/nginx/html/assets/images/signature/
CMD ["nginx", "-g", "daemon off;"]