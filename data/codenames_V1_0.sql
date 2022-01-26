/*DROP TABLE PARTIES;*/
DROP TABLE Utilisateurs;
DROP TABLE English;
DROP TABLE French;
DROP TABLE Langues;
DROP TABLE Themes;

/******************************************/
/*   DatabaseName = projet_TZ   */
/*   TableName =  Utilisateurs  */
/******************************************/

CREATE TABLE Utilisateurs
(
    id_utilisateur INT PRIMARY KEY NOT NULL AUTO_INCREMENT ,
    pseudo VARCHAR(20) NOT NULL ,
    mdp VARCHAR(20) NOT NULL,
    typ CHAR(1) CHECK  (typ IN ('A','E')),
    nb_partie_gagnee INT  NOT NULL,
    /*id_partie INT  NOT NULL,*/
    nb_carte_noir INT  NOT NULL
    /*CONSTRAINT fk_Utilisateurs_Parties FOREIGN KEY(id_partie) REFERENCES Parties(id_partie)*/
)ENGINE=InnoDB DEFAULT CHARSET=utf8 COMMENT='basic user information table for log in'
;


/*
/******************************************/
/*   DatabaseName = projet_TZ   */
/*   TableName =  Parties  */
/******************************************/
/*
REATE TABLE Parties
(
    id_partie INT PRIMARY KEY NOT NULL,
    theme  VARCHAR(20) NOT NULL,
    score INT NOT NULL,
    stat INT NOT NULL
);*/



/******************************************/
/*   DatabaseName = projet_TZ   */
/*   TableName =  Themes  */
/******************************************/
CREATE TABLE Themes
(
    id_theme INT PRIMARY KEY NOT NULL AUTO_INCREMENT,
    theme VARCHAR(20) UNIQUE NOT NULL
)ENGINE=InnoDB DEFAULT CHARSET=utf8 COMMENT='the theme of the words for player to choose'
;

/******************************************/
/*   DatabaseName = projet_TZ   */
/*   TableName =  Langues  */
/******************************************/
CREATE TABLE Langues
(
   id_langue INT PRIMARY KEY NOT NULL AUTO_INCREMENT,
   langue VARCHAR(20) UNIQUE NOT NULL
)ENGINE=InnoDB DEFAULT CHARSET=utf8 COMMENT='English & French'
;


/******************************************/
/*   DatabaseName = projet_TZ   */
/*   TableName =  French  */
/******************************************/
CREATE TABLE French
(
    id_mot INT PRIMARY KEY NOT NULL AUTO_INCREMENT,
    mot VARCHAR(20) NOT NULL,
    id_theme INT,
    id_langue INT,
    CONSTRAINT fk_french_theme FOREIGN KEY (id_theme) REFERENCES Themes(id_theme),
    CONSTRAINT fk_french_langue FOREIGN KEY (id_langue) REFERENCES Langues(id_langue)
      
)ENGINE=InnoDB DEFAULT CHARSET=utf8 COMMENT='the words set of French dictionary'
;


/******************************************/
/*   DatabaseName = projet_TZ   */
/*   TableName =  English */
/******************************************/
CREATE TABLE English
(
    id_word INT PRIMARY KEY NOT NULL AUTO_INCREMENT,
    word VARCHAR(20) NOT NULL,
    id_theme INT,
    id_langue INT,
    CONSTRAINT fk_english_theme FOREIGN KEY (id_theme) REFERENCES Themes(id_theme),
    CONSTRAINT fk_english_langue FOREIGN KEY (id_langue) REFERENCES Langues(id_langue)
      
)ENGINE=InnoDB DEFAULT CHARSET=utf8 COMMENT='the words set of English dictionary'
;


