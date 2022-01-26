package com.unistra.codenames.RootServer.DAO.Entities;

import lombok.AllArgsConstructor;
import lombok.Getter;
import lombok.Setter;

import javax.persistence.Entity;
import javax.persistence.GeneratedValue;
import javax.persistence.GenerationType;
import javax.persistence.Id;


@Entity(name = "Utilisateurs")
@Getter
@Setter

public class UserEntity {
    @Id
    @GeneratedValue(strategy = GenerationType.IDENTITY)
    private Integer id_utilisateur;

    private  String pseudo;
    private String mdp;
    private char typ;
    private Integer nb_partie_gagnee;
    /* private Integer id_partie; */
    private Integer nb_carte_noir;
}
