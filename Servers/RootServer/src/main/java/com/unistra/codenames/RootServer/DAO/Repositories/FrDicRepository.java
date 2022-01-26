package com.unistra.codenames.RootServer.DAO.Repositories;

import com.unistra.codenames.RootServer.DAO.Entities.FrDicEntity;
import com.unistra.codenames.RootServer.DAO.Entities.ThemesEntity;
import org.springframework.data.jpa.repository.Query;
import org.springframework.data.repository.CrudRepository;
import org.springframework.data.repository.query.Param;

import java.util.Optional;

public interface FrDicRepository extends CrudRepository<FrDicEntity, Integer> {

    @Query("select e from French e where e.themesEntity.id_theme = :idTheme")
    Optional<Iterable<FrDicEntity>> findAllById_theme(@Param("idTheme") Integer idTheme);

   // Optional<Iterable<FrDicEntity>> findAllByThemesEntityId_theme(Integer idTheme);

}
