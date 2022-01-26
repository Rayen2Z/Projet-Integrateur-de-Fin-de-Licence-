# La plan du serveur BDD de Huayi.T & Boan.T

## 1.le choix du technique 

### Le choix du langage pour construire serveur: NOSQL ou SQL (mysql,oracle,MongoDB)

Pour l'instant on a type de BDD SQL(relationnelle) et NOSQL (non relationnelle )comme option,  et ça depend de la conception de structure des données et le niveau de cohérence des données que l'équipe de BDD demande après ils analysent les "demande" estimatif  .si la structure des données de n'est pas fixe ou change fréquemment, NOSQL est un bon choix. en outre, si ils demandent haut niveau cohérence des données, type SQL est meilleur.

Et pour l'instant Boan et moi on a 3 langage representatif qu'on suppose que la majorité ont de connaissance basique et c'est le condition nécessaire qu'on peux construire de serveur de BDD et faire l'interaction avec eux, si vous connaissez de langage plus

| La langage | Avantage                                                     | Désavantage                                                  |
| ---------- | ------------------------------------------------------------ | ------------------------------------------------------------ |
| mysql      | 1.multi-thread,multi-utilisateur<br />2.BDD relationnelle<br />3.Authentification basée sur l'hôte<br />4.Disponible même sans Internet<br />5.Fournir un serveur en tant que programme autonome pour l'environnement réseau client / serveur<br /> | 1.lorsque la quantité de données atteint une certaine échelle, afin de maintenir la cohérence, elle est très sujette à des problèmes de concurrence tels que des blocages, de sorte que la vitesse de lecture et d'écriture diminue très sérieusement<br />2.Requête dans une table contenant une grande quantité de données, l'efficacité est très faible |
| Oracle     | 1.Compatible avec IBM SQL / DS, DB2, INGRES, IDMS / R, etc.<br />2.BDD relationnelle<br />3. Il peut fonctionner sous différents systèmes tels que VMS, DOS, UNIX et Windows.<br /> | Presque pareil que celles de  mysql                          |
| MongoDB    | 1.Face au document<br />2.BDD non relationnelle<br />3.Pas besoin de passer par l'analyse de la couche SQL, performances de lecture et d'écriture élevées.<br />4.Sur la base de paires clé-valeur, les données ne sont pas couplées et faciles à développer.<br />5.Formats de stockage multiples: prennent en charge le format clé-valeur, le format de document et le format d'image, tandis que les bases de données relationnelles ne prennent en charge que les types de base;<br />6.Un bon choix de serveur de cache | 1.L'avantage est au détriment du principe ACID:bien que cette solution augmente considérablement le temps disponible et l'évolutivité, elle peut également provoquer une perte de données<br />2.MongoDB ne soutiens pas  les opérations de jointure et il n'est pas recommandé aux applications qui nécessitent des requêtes complexes. |

### 2.La langage  de l'interaction de backend serveur et BDD:  JAVA

Justification: c'est simple, le backend programmer dans ce projet utilise Java





## 2.Timelines de nos travail(préliminaire**):

```javascript
 2 / 2:Confirmation de “Cahier de charges” avec professeur

 7/ 2: Communiqué avec l équipe de BDD pour trouver un compromis entre nous,(laquelle API on va utiliser, et connaître la structure des données ils desigent...)

13 / 2:Confirmation du mécanisme de l interaction (Avec Backend Programmeur Chenglin.X)

16 / 2 Conception du prototype de serverur BDD et trouver la possibilité de réaliser solution potentielle optimisé par exemple  "Read/Write Splitting"

22 / 2 - 3 /3:Réalisation de serveur BDD

3 / 3 - 9 / 3 : cohérer le serveur avec le BDD qu ils vont créer 

9 / 3 - 23 / 3:Test d’intégration , et essayer de faire optimisation .
```





## 3.Pourquoi on a besoins d'un serveur de BDD:

Quand on a étais au semestre 5,  on a entendu une nouvelle chinois :Une fameux website qui s'apelle "IT House"  migre les serveurs d'Alibaba Cloud vers Baidu Cloud, mais "ITHouse" est un siteweb .net, un serveur de serveur SQL autonome, et il tous fonctionne sur le même serveur", et vue que séparation le serveur du web et serveur de BDD est le "design du mode" qui s'est appliqué par des entreprise fameux comme Baidu Google...   ça nous fait réfléchir et on a fait quelque recherche  , et on a constaté que même si le processus ".net" est entièrement chargé, seuls 25 à 50% du processeur peuvent être utilisés sur un serveur quadricœur. Si le Web et la BDD sont déployés sur la même machine, ils utiliseront pleinement toutes les ressources informatiques en 50% chacun.  du coup on fait une hyphthèse "stupid" :Déployer le Web et la BDD séparement sur un serveur de 10 00 euros, et tout déployer sur un serveur de 20 00 euros, les performances ne devraient-elles pas être les mêmes en théorie? et cela réduit également le temps de communication entre la base de données et le Web. lié au problème d'utilisation du processeur que j'ai mentionné ci-dessus, en théorie, lorsque le coût total reste le même, il devrait être plus pratique de déployer sur une seule machine à haute configuration que de déployer sur plusieurs machines séparément.

Et puis , moi et Boan on a discuté la sur stratégie optimisé en demandant aux gens qui travaillent dans cette domaine depuis longtemps, on a conclusion que "séparation le serveur du web et serveur de BDD"  est encore la solution de l'efficacité  "1+1>2" et les raisons sont suivant:

1. Fondamentalement, les logiciels à grande échelle sont de nos jours engagés dans le déploiement et l'informatique distribués. Le plus tabou dans l'environnement de production est un point de défaillance unique. Même un simple microservice informatique doit avoir au moins deux copies, et il est préférable d'exiger ces copies doivent être dans différentes machines, dans différents racks, ou même dans différents centres de données, de sorte qu'en cas d'accident, une autre copie valide puisse être connectée pour continuer à travailler immédiatement. Ceci est vrai pour un service simple, sans parler d'un serveur Web lourd Il est extrêmement déraisonnable de déployer sur la même machine physique que le serveur de base de données

2. Ils utilisent différentes ressources logicielles et matérielles avec une efficacité différente. Le serveur Web est principalement utilisé pour traiter les connexions réseau et les demandes de ressources. Les exigences sont donc une bande passante élevée et une grande concurrence. Les exigences en matière de processeur ne sont en fait pas élevées et les exigences en matière de mémoire sont élevées, car une grande quantité d'informations et de pools de threads doivent être mis en cache à ce niveau dans un souci de rapidité, il ne nécessite pas d'E / S disque élevé, il peut donc être spécifiquement optimisé pour les serveurs multicœurs avec une grande mémoire. 

   Cependant, l'optimisation pour les serveurs Web n'est évidemment pas adaptée aux serveurs de bases de données. La principale responsabilité du serveur de base de données est de traiter les instructions SQL et de gérer les données stockées sur le disque. Il nécessite une grande quantité d'E / S disque et des exigences extrêmement élevées sur le pool de mémoire tampon, mais le degré de concurrence est bien inférieur à celui de le serveur Web. 

   En résumé, le serveur Web et le serveur de base de données sont positionnés différemment, et les points d'optimisation sont également différents. Les forcer ensemble affectera gravement les performances des deux.

3. Pour la raison de sécurité,généralement, les BDD d'entreprise sont déployées sur l'intranet et les ports ne seront pas ouverts pour empêcher les attaques de pirates. Elles ne sont accessibles que via intranet et gérées par un DBA professionnel, mais le serveur Web est ouvert.

Et comme on a chance de avoir expérience de travailler comme un vrai équipe avec tous le monde pour finaliser le projet de nous-meme,nous espérons que notre modèle de développement de modèle de travail est plus proche de la situation d'application dans la vie réelle. 