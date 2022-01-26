package com.unistra.codenames.RootServer.DAO.Entities;

import lombok.Getter;
import lombok.Setter;

import javax.persistence.Entity;
import javax.persistence.GeneratedValue;
import javax.persistence.GenerationType;
import javax.persistence.Id;

@Entity(name="Themes")
@Getter
@Setter
public class ThemesEntity {
    @Id
    @GeneratedValue(strategy = GenerationType.IDENTITY)
    Integer id_theme;
    String theme;
}
