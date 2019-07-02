#!/bin/bash

if [ $# -lt 1 ]
then
        echo "Usage : $0 name"
        exit 1
fi
case "$1" in
"clients")  echo "ctx: $1"
    dotnet ef dbcontext info -c IdentityServer4.EntityFramework.DbContexts.ConfigurationDbContext
    ;;
"users")  echo  "ctx: $1"
    dotnet ef dbcontext info -c byappt_identity.Data.UserStoreDbContext
    ;;
"grants")  echo  "ctx: $1"
    dotnet ef dbcontext info -c IdentityServer4.EntityFramework.DbContexts.PersistedGrantDbContext
    ;;
*) echo "valid ctx names: clients users grants"
   ;;
esac
exit 0
