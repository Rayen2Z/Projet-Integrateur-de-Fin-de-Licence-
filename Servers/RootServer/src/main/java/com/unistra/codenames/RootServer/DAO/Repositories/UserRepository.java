package com.unistra.codenames.RootServer.DAO.Repositories;

import com.unistra.codenames.RootServer.DAO.Entities.UserEntity;
import org.springframework.data.repository.CrudRepository;

import java.util.Optional;

public interface UserRepository extends CrudRepository<UserEntity, Integer> {
    @Override
    Optional<UserEntity> findById(Integer integer);
    Optional<UserEntity> findByPseudo(String pseudo);
    boolean existsByPseudo(String pseudo);
}
