package com.unistra.codenames.RootServer.DAO.Entities;

import lombok.Getter;
import lombok.Setter;

import javax.persistence.*;

@Getter
@Setter
@Entity(name="French")
public class FrDicEntity {
    @Id
    @Column(name = "id_mot")
    @GeneratedValue(strategy = GenerationType.IDENTITY)
    private Integer id_word;

    @Column(name = "mot")
    private String word;

    @ManyToOne
    @JoinColumn(name = "id_theme", referencedColumnName = "id_theme")
    private ThemesEntity themesEntity;

    @ManyToOne
    @JoinColumn(name = "id_langue", referencedColumnName = "id_langue")
    private LanguageEntity languageEntity;
}
