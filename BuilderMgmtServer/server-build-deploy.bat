cls
set name=bmgmt-server-image:v15
set rname=vmikeska/%name%
echo %name% 
echo %rname%
docker build -t %rname% . & docker push %rname%
