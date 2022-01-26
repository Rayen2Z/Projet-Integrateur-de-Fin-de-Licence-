package com.unistra.codenames.RootServer.DAO.Repositories;

import com.unistra.codenames.RootServer.DAO.Entities.EnDicEntity;
import com.unistra.codenames.RootServer.DAO.Entities.ThemesEntity;
import org.springframework.data.jpa.repository.Query;
import org.springframework.data.repository.CrudRepository;
import org.springframework.data.repository.query.Param;

import java.util.Optional;

public interface EnDicRepository extends CrudRepository<EnDicEntity, Integer> {

    @Query("select e from English e where e.themesEntity.id_theme = :idTheme")
    Optional<Iterable<EnDicEntity>> findAllById_theme(@Param("idTheme")Integer idTheme);

    //Optional<Iterable<EnDicEntity>> findAllByThemesEntityId_theme(Integer idTheme);
}
